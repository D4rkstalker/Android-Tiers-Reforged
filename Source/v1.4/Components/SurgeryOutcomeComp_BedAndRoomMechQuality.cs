﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ATReforged
{
    // Override AffectQuality to use MechanicalSurgerySuccessChanceFactor instead of SurgerySuccessChanceFactor
    public class SurgeryOutcomeComp_BedAndRoomMechQuality : SurgeryOutcomeComp_BedAndRoomQuality
    {
        
        public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
        {
            quality *= patient.CurrentBed().GetStatValue(StatDefOf.MechanicalSurgerySuccessChanceFactor);
        }
    }
}