﻿using RimWorld;
using Verse;
using Verse.AI;

namespace ATReforged
{
    // Mechanical units should attempt maintenance if in poor maintenance and allowed to gain enough to escape it before doing other work.
    public class JobGiver_DoMaintenanceUrgent : ThinkNode_JobGiver
    {
        // Pawn ThinkTrees occasionally sort jobs to take on a priority. This is a high priority job, and should almost always be done ahead of work.
        public override float GetPriority(Pawn pawn)
        {
            TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
            {
                return 9.25f;
            }
            else if (timeAssignmentDef == TimeAssignmentDefOf.Work)
            {
                return 8f;
            }
            else if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
            {
                return 9.25f;
            }
            else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
            {
                return 8f;
            }
            else if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
            {
                return 11f;
            }
            return 0.5f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            CompMaintenanceNeed compMaintenanceNeed = pawn.GetComp<CompMaintenanceNeed>();

            if (compMaintenanceNeed == null || pawn.InAggroMentalState || !pawn.Spawned)
            {
                return null;
            }

            // Urgent maintenance is not done if the target level is below the poor maintenance threshold.
            if (compMaintenanceNeed.TargetMaintenanceLevel <= 0.3)
            {
                return null;
            }

            // Urgent maintenance is done only if below poor maintenance or if maintenance level is at least 5% lower than the desired level.
            if (compMaintenanceNeed.Stage > CompMaintenanceNeed.MaintenanceStage.Poor && compMaintenanceNeed.MaintenanceLevel >= compMaintenanceNeed.TargetMaintenanceLevel - 0.05)
            {
                return null;
            }

            // If the pawn can not do meditation for medical reasons, don't do maintenance.
            if (!MeditationUtility.CanMeditateNow(pawn))
            {
                return null;
            }

            // If this pawn's current position is legal for meditation, use it.
            if (ReservationUtility.CanReserve(pawn, pawn.Position) && MeditationUtility.SafeEnvironmentalConditions(pawn, pawn.Position, pawn.Map))
            {
                return JobMaker.MakeJob(ATR_JobDefOf.ATR_DoMaintenanceUrgent, pawn.Position, pawn.InBed() ? ((LocalTargetInfo)pawn.CurrentBed()) : new LocalTargetInfo(pawn.Position));
            }

            // FindMeditationSpot will find a place that is valid and will allow this job to continue. If it is invalid, then there is nowhere to do maintenance and no job is given.
            MeditationSpotAndFocus meditationSpot = MeditationUtility.FindMeditationSpot(pawn);
            if (meditationSpot.IsValid)
            {
                return JobMaker.MakeJob(ATR_JobDefOf.ATR_DoMaintenanceUrgent, meditationSpot.spot, new LocalTargetInfo(meditationSpot.spot.Cell));
            }
            return null;
        }
    }
}