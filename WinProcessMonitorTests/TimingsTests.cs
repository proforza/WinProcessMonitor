using NUnit.Framework;
using WinProcessMonitor;
using System.Diagnostics;
using System.Threading;
using System;

namespace WinProcessMonitorTests
{
    [TestFixture]
    public class TimingsTests
    {
        // these variables can be like input parameters
        private static string processName = "notepad";
        private static int lifeTime = 1;
        private static int testTimeout = 2;

        [Test, Order(1)]
        public void Process_ShouldBe_Started()
        {
            // Arrange
            Process process = new();
            process.StartInfo.FileName = $"{processName}.exe";

            // Act
            process.Start();

            // Assert
            Assert.That(Process.GetProcessesByName(processName).Length > 0);
        }

        [Test, Order(2)]
        public void Process_ShouldBe_Killed_After_Timeout()
        {
            // Arrange
            bool isProcessExist = true;
            DateTime monitorStartTime = DateTime.Now;

            // Act
            while (isProcessExist && (monitorStartTime.AddMinutes(testTimeout) > DateTime.Now)) // second condition is to avoid infinite loop in case if notepad.exe will never be closed
            {
                Program.KillProcessByLifetime(processName, lifeTime);
                Thread.Sleep(5000);
                isProcessExist = !(Process.GetProcessesByName(processName).Length == 0);
            }

            // Assert
            Assert.That(!isProcessExist);
        }
    }
}