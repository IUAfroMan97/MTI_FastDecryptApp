// Decompiled with JetBrains decompiler
// Type: Tools.Impersonator
// Assembly: FastDecryptApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 707BCE7B-2368-4313-AB42-2091C3748DE7
// Assembly location: C:\Users\jgood\Desktop\FastDecryptApp2.0.0.exe

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Tools
{
  public class Impersonator : IDisposable
  {
    private WindowsImpersonationContext impersonationContext = (WindowsImpersonationContext) null;
    private const int LOGON32_LOGON_INTERACTIVE = 2;
    private const int LOGON32_PROVIDER_DEFAULT = 0;

    public Impersonator(string userName, string domainName, string password)
    {
      this.ImpersonateValidUser(userName, domainName, password);
    }

    public void Dispose()
    {
      this.UndoImpersonation();
    }

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int LogonUser(string lpszUserName, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseHandle(IntPtr handle);

    private void ImpersonateValidUser(string userName, string domain, string password)
    {
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      try
      {
        if (!Impersonator.RevertToSelf())
          throw new Win32Exception(Marshal.GetLastWin32Error());
        if ((uint) Impersonator.LogonUser(userName, domain, password, 2, 0, ref zero1) <= 0U)
          throw new Win32Exception(Marshal.GetLastWin32Error());
        if ((uint) Impersonator.DuplicateToken(zero1, 2, ref zero2) <= 0U)
          throw new Win32Exception(Marshal.GetLastWin32Error());
        this.impersonationContext = new WindowsIdentity(zero2).Impersonate();
      }
      finally
      {
        if (zero1 != IntPtr.Zero)
          Impersonator.CloseHandle(zero1);
        if (zero2 != IntPtr.Zero)
          Impersonator.CloseHandle(zero2);
      }
    }

    private void UndoImpersonation()
    {
      if (this.impersonationContext == null)
        return;
      this.impersonationContext.Undo();
    }
  }
}
