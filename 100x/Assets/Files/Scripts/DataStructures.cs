using System;
using UnityEngine;

public class DataStructures : MonoBehaviour
{

}





public class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapper = "{\"array\":" + json + "}";
        Wrapper<T> wrapperObject = JsonUtility.FromJson<Wrapper<T>>(wrapper);
        return wrapperObject.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}


[Serializable]
public class DataUser
{
    public string name;
    public int coins;
    public int reward_coins;
    public int total_winnings;
    public int total_money_added;
    public string email;
    public string pan_number;
    public string contact_number;
    public string referral_code;
    public string picture;
    public string created_at;
    public string updated_at;
    public string access_token;
}


[Serializable]
public class GamesInfoData
{
    public string game_type;
    public string channel_id;
    public string auth_token;
    public float thread_count;
    public float game_wait;
    public float thread_time;
}

[Serializable]
public class DataGameInfo
{
    public GamesInfoData[] games_info;
    public string websocket_url;
}


[Serializable]
public class DataAuth
{
    public string name;
    public int coins;
    public int total_winnings;
    public int total_money_added;
    public string email;
    public string picture;
    public DateTime created_at;
    public DateTime updated_at;
    public string access_token;
}


[Serializable]
public class PropsWebSocket
{
    public bool disable_group_highlight;
}

[Serializable]
public class MetadataWebSocket
{

}

[Serializable]
public class PostWebSocket
{
    public string id;
    public long create_at;
    public long update_at;
    public int edit_at;
    public int delete_at;
    public bool is_pinned;
    public string user_id;
    public string channel_id;
    public string root_id;
    public string original_id;
    public string message;
    public string type;
    public PropsWebSocket props;
    public string hashtags;
    public string pending_post_id;
    public int reply_count;
    public long last_reply_at;
    public object participants; // You may need to replace 'object' with the actual type
    public MetadataWebSocket metadata;
}

[Serializable]
public class DataWebSocketChannel
{
    public string channel_display_name;
    public string channel_name;
    public string channel_type;
    public string post;
    public string sender_name;
    public bool set_online;
    public string team_id;
}

[Serializable]
public class BroadcastWebSocket
{
    // You can define additional fields if needed
}

[Serializable]
public class DataWebSocket
{
    public string @event;
    public DataWebSocketChannel data;
    public BroadcastWebSocket broadcast;
    public int seq;
}


[Serializable]
public class DataGameBet
{
    public int remaining_coins;
}

[Serializable]
public class DataNewGame
{
    public string game_type;
    public string game_id;
    public string created_at;
}