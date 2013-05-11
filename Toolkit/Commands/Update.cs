namespace ClariusLabs.NuGet.Toolkit.Commands
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using Clide.Commands;

    [Command(Guids.PackageGuid, Guids.CommandSetGuid, CommandIds.Update)]
    public class Update : ICommandExtension
    {
        private static readonly ITracer tracer = Tracer.Get<Update>();

        public void Execute(IMenuCommand command)
        {
            tracer.Info("Update");

            MessageBox.Show("Updating package...");
        }

        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = command.Visible = true;
        }

        public string Text { get; set; }
    }
}