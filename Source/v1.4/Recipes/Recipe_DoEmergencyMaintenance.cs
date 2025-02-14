﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ATReforged
{
    public class Recipe_DoEmergencyMaintenance : Recipe_SurgeryAndroids
    {
        // This recipe always targets the core part. It is valid if the pawn has the CompMaintenanceNeed with a level lower than 40%.
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            if (pawn.GetComp<CompMaintenanceNeed>()?.MaintenanceLevel < 0.4f)
                yield return pawn.RaceProps.body.corePart;
        }

        // On completion, increase the maintenance level by 10% up to a max of 40% overall.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            CompMaintenanceNeed compMaintenanceNeed = pawn.GetComp<CompMaintenanceNeed>();
            if (compMaintenanceNeed.MaintenanceLevel >= 0.3f)
            {
                compMaintenanceNeed.ChangeMaintenanceLevel(0.4f - compMaintenanceNeed.MaintenanceLevel);
            }
            else
            {
                compMaintenanceNeed.ChangeMaintenanceLevel(0.1f);
            }
        }
    }
}