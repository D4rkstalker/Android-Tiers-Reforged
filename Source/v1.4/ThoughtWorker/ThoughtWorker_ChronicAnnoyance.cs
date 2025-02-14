﻿using RimWorld;
using Verse;

namespace ATReforged
{
    public class ThoughtWorker_ChronicAnnoyance : ThoughtWorker
    {
        public static ThoughtState CurrentThoughtState(Pawn p)
        {
            int rustedSeverity = 0;
            for (int i = p.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
            {
                if (p.health.hediffSet.hediffs[i].def.chronic)
                    rustedSeverity++;
            }

            switch (rustedSeverity)
            {
                case 0:
                    return ThoughtState.Inactive;
                case 1:
                case 2:
                case 3:
                    return ThoughtState.ActiveAtStage(rustedSeverity - 1);
                default:
                    return ThoughtState.ActiveAtStage(3);
            }
        }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (ThoughtUtility.ThoughtNullified(p, def) || !Utils.IsConsideredMechanical(p))
            {
                return ThoughtState.Inactive;
            }
            return CurrentThoughtState(p);
        }
    }
}
