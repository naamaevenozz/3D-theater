namespace Edelweiss.Core
{
    public class GameStateMachine
    {
        public enum GameState
        {
            None,
            MainLoop,
            Title,
            GameOver,
            Win
        }

        private GameState m_State = GameState.None;
        public  GameState CurrentState => m_State;

        public bool SetState(GameState newState)
        {
            bool validTransition = false;

            switch (newState)
            {
                case GameState.None:
                    validTransition = false;
                    break;
                case GameState.Title:
                    validTransition = m_State == GameState.None;
                    break;
                case GameState.GameOver:
                    validTransition = m_State == GameState.MainLoop;
                    break;
                case GameState.MainLoop:
                    validTransition = true;
                    break;
                case GameState.Win:
                    validTransition = m_State == GameState.MainLoop;
                    break;
            }

            m_State = validTransition ? newState : m_State;

            return validTransition;
        }
    }
}