using BombermanServer.Hubs;
using Microsoft.AspNetCore.Builder;

namespace BombermanServer.Configurations
{
    public static class HubConfiguration
    {
        public static void ConfigureHubs(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UserHub>("/user-hub"); // ClientSide will connect to localhost:5001/user-hub before sending any requests.
            });
        }
    }
}
