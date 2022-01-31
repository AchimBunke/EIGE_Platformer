

using System;

public class GameData
{
    private static GameData instance;
    private int score = 0;
    private int lives = 3;

    private GameData()
    {
        if (instance != null) return;
        instance = this;
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }
    
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = Math.Max(0,value);
        }
    }

        public int Lives
    {
        get;
        set;
    }
}
