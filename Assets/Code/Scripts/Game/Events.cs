namespace Unity.Ricochet.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static AllEnemiesKilled AllEnemiesKilled = new AllEnemiesKilled();
    }

    public class PlayerDeathEvent : GameEvent { }
    public class GameOverEvent : GameEvent { }
    public class AllEnemiesKilled : GameEvent { }
}
