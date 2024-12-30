using Serilog;
using Serilog.Events;
using Serilog.Templates.Themes;
using SerilogTracing;
using SerilogTracing.Expressions;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
    .WriteTo.Console(Formatters.CreateConsoleTextFormatter(TemplateTheme.Code))
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

using var _ = new ActivityListenerConfiguration().TraceToSharedLogger();

var activitySource = new ActivitySource("ECommerceOrderProcessor");

var orderProcessor = new OrderProcessor(activitySource);

for (int i = 0; i < 5; i++)
{
    orderProcessor.ProcessOrder(i);
}
