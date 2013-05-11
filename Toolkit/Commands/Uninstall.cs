namespace ClariusLabs.NuGet.Toolkit.Commands
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using Clide.Commands;

    [Command(Guids.PackageGuid, Guids.CommandSetGuid, CommandIds.Uninstall)]
    public class Uninstall : ICommandExtension
    {
        private static readonly ITracer tracer = Tracer.Get<Uninstall>();

        public void Execute(IMenuCommand command)
        {
            tracer.Info("Uninstall");

            MessageBox.Show("Uninstalling package...");
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = command.Visible = true;
        }

        public string Text { get; set; }
    }
}