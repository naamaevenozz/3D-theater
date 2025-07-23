using System;
using Edelweiss.Items;
using Edelweiss.Damage;
using UnityEngine;

namespace Edelweiss.Core
{
    public class EDEEventManager
    {
        private static EDEEventManager _instance;

        public static EDEEventManager Instance
        {
            get
            {
                if (_instance == null) _instance = new EDEEventManager();

                return _instance;
            }
        }

        /* TITLE SCREEN */
        public Action TitleScreenInit     = delegate { };
        public Action OnTitleScreenLoaded = delegate { };

        /* GAME SCENE */
        public Action            GameSceneInit   = delegate { };
        public Action            FightStart      = delegate { };
        public Action            FightEnd        = delegate { };
        public Action<Collider> OnKissCollision = delegate { };

        /* GAME SCENE: PLAYERS */
        public Action<HitContext>          HitRegistered = delegate { };
        public Action<HitContext>          HitApplied    = delegate { };
        public Action<HealthChangeContext> HealthChanged = delegate { };
        public Action<ItemGrabContext>     ItemGrabbed   = delegate { };
    }
}