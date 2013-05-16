namespace ClariusLabs.NuGetToolkit.VsCommands
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using Clide.Commands;

    [Command(Guids.PackageGuid, Guids.CommandSetGuid, CommandIds.Reinstall)]
    public class Reinstall : ICommandExtension
    {
        private static readonly ITracer tracer = Tracer.Get<Reinstall>();

        public void Execute(IMenuCommand command)
        {
            tracer.Info("Reinstall");

            MessageBox.Show("Reinstalling package...");
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = command.Visible = false;
        }

        public string Text { get; set; }
    }
}