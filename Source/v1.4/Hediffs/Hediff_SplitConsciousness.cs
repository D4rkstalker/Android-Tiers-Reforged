﻿using System.Linq;
using UnityEngine;
using Verse;

namespace ATReforged
{
    // This Hediff class appears only on SkyMind connected pawns, and acts as a penalty for a physical pawn attempting to control too many surrogates at once.
    public class Hediff_SplitConsciousness : HediffWithComps
    {
        public override bool ShouldRemove => !pawn.GetComp<CompSkyMindLink>().HasSurrogate();

        public override void PostTick()
        {
            base.PostTick();
            if (!pawn.IsHashIntervalTick(180))
                return;

            CompSkyMindLink link = pawn.GetComp<CompSkyMindLink>();

            // Surrogates need the CompSkyMindLink of their controller.
            if (Utils.IsSurrogate(pawn))
            {
                SetSeverity(link.GetSurrogates().First().GetComp<CompSkyMindLink>().GetSurrogates().Count());
            }
            else
            {
                SetSeverity(link.GetSurrogates().Count());

            }
        }

        // Severity of this hediff is controlled by how many pawns are linked together via this controller (or this surrogate's controller).
        // If there are fewer or equal pawns to the soft cap, there is no penalty. Otherwise the scale of penalty is based on 5 stages, and no stage may be skipped.
        private void SetSeverity(int pawnCount)
        {
            int softCap = ATReforged_Settings.safeSurrogateConnectivityCountBeforePenalty;

            // If underneath the cap, there is no penalty. 
            if (pawnCount <= softCap)
            {
                Severity = 0.01f;
            }
            // Penalty should not skip any of the 5 stages, so if the cap is small, do simplified math.
            else if (softCap <= 5)
            {
                Severity = Mathf.Clamp(0.2f * (pawnCount - softCap), 0.01f, 1f);
            }
            // Penalty should be scaled by the softCap so having a large softCap means more surrogates with less harsh penalties over time.
            else
            {
                Severity = Mathf.Clamp((pawnCount - softCap) / ((float)softCap), 0.01f, 1f);
            }
        }
    }
}
