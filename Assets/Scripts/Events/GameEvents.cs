using Ducksten.ZombieShooterTT.Enemies;

namespace Ducksten.ZombieShooterTT.Events {
    //#TODO: Maybe remake that into EventSystem<> that would deal with events based on type of object given to it?
    //#TODO: Or just split events into few categories (input gameplay etc) and put them into appropriate static classes
    public static class GameEvents {
        //Gameplay
        public static GameEvent<Enemy> onEnemyDied = new();
        public static GameEvent onGameEnded = new();
        public static GameEvent onGameRestarted = new();
    }
}
