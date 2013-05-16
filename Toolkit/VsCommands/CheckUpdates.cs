namespace ClariusLabs.NuGetToolkit.VsCommands
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using Clide.Commands;

    [Command(Guids.PackageGuid, Guids.CommandSetGuid, CommandIds.CheckUpdates)]
    public class CheckUpdates : ICommandExtension
    {
        private static readonly ITracer tracer = Tracer.Get<CheckUpdates>();

        public CheckUpdates()
        {
        }

        public void Execute(IMenuCommand command)
        {
            MessageBox.Show("Checking for updates");
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = command.Visible = true;
        }

        public string Text { get; set; }
    }
}