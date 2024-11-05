using PriceListener.Application.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace PriceListener.Presentation.Terminal
{
    [ExcludeFromCodeCoverage]
    public class WorkerService
    {
        private readonly IPriceListenerService priceListenerService;
        private readonly CancellationTokenSource cancellationTokenSource;

        private const string MSG_PRESS_KEY_TO_END_PROGRAM = "Press any key to end program...";
        private const string MSG_TASKS_FINISHED = "Tasks completed";

        public WorkerService(
            IPriceListenerService priceListenerService,
            CancellationTokenSource cancellationTokenSource
        )
        {
            this.priceListenerService = priceListenerService;
            this.cancellationTokenSource = cancellationTokenSource;
        }
    
        public async Task Run()
        {
            List<Action> actions = new() {
                () => this.priceListenerService.StartReceivingCryptoDataAsync(),
                () => this.priceListenerService.MonitorAndSaveEnqueuedDataAsync(),
                () => this.priceListenerService.DisplayInfoAsync()
                };

            Parallel.ForEach(actions, action => action());

            Console.WriteLine(MSG_PRESS_KEY_TO_END_PROGRAM);
            Console.ReadKey();

            await this.priceListenerService.StopReceivingCryptoDataAsync();
            cancellationTokenSource.Cancel();

            Console.WriteLine(MSG_TASKS_FINISHED);
        }
    }
}
