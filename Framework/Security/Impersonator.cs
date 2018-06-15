using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Framework.Security
{
    /// <summary>
    ///     Impersonation of a user. Allows to execute code under another
    ///     user context.
    ///     Please note that the account that instantiates the Impersonator class
    ///     needs to have the 'Act as part of operating system' privilege set.
    /// </summary>
    /// <remarks>
    ///     This class is based on the information in the Microsoft knowledge base
    ///     article http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306158
    ///     Encapsulate an instance into a using-directive like e.g.:
    ///     ...
    ///     using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
    ///     {
    ///     ...
    ///     [code that executes under the new context]
    ///     ...
    ///     }
    ///     ...
    /// </remarks>
    public class Impersonator : IDisposable
    {
        #region IDisposable

        /// <summary>
        ///     Undo the impersonation
        /// </summary>
        public void Dispose()
        {
            UndoImpersonation();
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Constructor. Starts the impersonation with the given credentials.
        ///     Please note that the account that instantiates the Impersonator class
        ///     needs to have the 'Act as part of operating system' privilege set.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        public Impersonator(string userName, string domainName, string password)
        {
            ImpersonateValidUser(userName, domainName, password);
        }

        /// <summary>
        ///     Constructor. Starts the impersonation with the given credentials.
        ///     Please note that the account that instantiates the Impersonator class
        ///     needs to have the 'Act as part of operating system' privilege set.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">
        ///     The domain name of the user to act as. also can be the name of the server the account resides
        ///     on
        /// </param>
        /// <param name="password">The password of the user to act as.</param>
        /// <param name="newCredentials">Set to </param>
        public Impersonator(string userName, string domainName, string password, bool newCredentials)
        {
            ImpersonateValidUser(userName, domainName, password, LOGON32_LOGON_NEW_CREDENTIALS);
        }

        #endregion

        #region P/Invoke

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(
            IntPtr handle);

        private const int LOGON32_PROVIDER_DEFAULT = 0;

        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        private const int LOGON32_LOGON_INTERACTIVE = 2;

        #endregion

        #region Private member.

        /// <summary>
        ///     Does the actual impersonation.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domain">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        /// <param name="provider">The provider to use</param>
        private void ImpersonateValidUser(string userName, string domain, string password, int provider)
        {
            WindowsIdentity tempWindowsIdentity = null;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            try
            {
                if (RevertToSelf())
                {
                    if (LogonUser(
                            userName,
                            domain,
                            password,
                            provider,
                            LOGON32_PROVIDER_DEFAULT,
                            ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (token != IntPtr.Zero) CloseHandle(token);

                if (tokenDuplicate != IntPtr.Zero) CloseHandle(tokenDuplicate);
            }
        }

        /// <summary>
        ///     Does the actual impersonation.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domain">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        private void ImpersonateValidUser(string userName, string domain, string password)
        {
            ImpersonateValidUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE);
        }

        /// <summary>
        ///     Reverts the impersonation.
        /// </summary>
        private void UndoImpersonation()
        {
            impersonationContext?.Undo();
        }

        private WindowsImpersonationContext impersonationContext;

        #endregion
    }
}