public class APIs
{
    private readonly static string baseURL = "https://server.100xgame.com/";

    internal readonly static string login = baseURL + "api/auth/login";
    internal readonly static string google = baseURL + "api/auth/google";
    internal readonly static string register = baseURL + "api/auth/register";
    internal readonly static string leaderboard = baseURL + "api/user/leaderboard";
    internal readonly static string support = baseURL + "api/support";
    internal readonly static string submitRefral = baseURL + "/api/user/referral";
    internal readonly static string refral = baseURL + "api/referal";
    internal readonly static string user = baseURL + "api/user";
    internal readonly static string gameInfo = baseURL + "api/game_info";
    internal readonly static string getUrl = baseURL + "api/user/app/wallet";
    internal readonly static string history = baseURL + "api/user/history?limit=10&page=";
    internal readonly static string editUser = baseURL + "api/user/edit";
    internal readonly static string gameHistory = baseURL + "api/games/";
    internal readonly static string gameBid = baseURL + "api/game/bid";
    internal readonly static string crashCashOut = baseURL + "api/game/transact-out";
}