namespace GuessOrLose.EventHandler
{
    public static class EventHandlerProviderDIExtensions
    { 
        public static IServiceCollection AddEventHandlerProvider(this IServiceCollection services)
        {
            services.AddTransient(typeof(IEventHandler<>), typeof(EventHandler<>));
            services.AddTransient(typeof(IEventHandlerProvider<>), typeof(EventHandlerProvider<>));
            return services;
        }
    }
}
