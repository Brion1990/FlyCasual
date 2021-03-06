﻿using Upgrade;
using System.Collections.Generic;
using System.Linq;
using Ship;
using ActionsList;
using Obstacles;
using BoardTools;
using Actions;

namespace UpgradesList.SecondEdition
{
    public class DebrisGambit : GenericUpgrade
    {
        public DebrisGambit() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Debris Gambit",
                UpgradeType.Talent,
                cost: 2,
                abilityType: typeof(Abilities.SecondEdition.DebrisGambit),
                restriction: new BaseSizeRestriction(BaseSize.Small, BaseSize.Medium),
                addAction: new ActionInfo(typeof(EvadeAction), ActionColor.Red),
                seImageNumber: 3
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DebrisGambit : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckActionComplexity += CheckDecreaseComplexity;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckActionComplexity -= CheckDecreaseComplexity;
        }

        private void CheckDecreaseComplexity(ref GenericAction action)
        {
            if (action is EvadeAction && action.IsRed)
            {
                if (IsNearObstacle()) action.IsRed = false;
            }
        }

        private bool IsNearObstacle()
        {
            if (HostShip.IsLandedOnObstacle)
            {
                Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + ": Action is treated as white");
                return true;
            }

            foreach (GenericObstacle obstacle in ObstaclesManager.GetPlacedObstacles())
            {
                ShipObstacleDistance shipObstacleDist = new ShipObstacleDistance(HostShip, obstacle);
                if (shipObstacleDist.Range < 2)
                {
                    Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + ": Action is treated as white");
                    return true;
                }
            }

            return false;
        }
    }
}