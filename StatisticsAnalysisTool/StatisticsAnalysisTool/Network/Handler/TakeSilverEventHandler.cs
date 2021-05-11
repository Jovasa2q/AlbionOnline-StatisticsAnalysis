﻿using Albion.Network;
using StatisticsAnalysisTool.Common;
using StatisticsAnalysisTool.Enumerations;
using StatisticsAnalysisTool.Network.Controller;
using StatisticsAnalysisTool.Network.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValueType = StatisticsAnalysisTool.Enumerations.ValueType;

namespace StatisticsAnalysisTool.Network.Handler
{
    public class TakeSilverEventHandler : EventPacketHandler<TakeSilverEvent>
    {
        private readonly TrackingController _trackingController;

        public TakeSilverEventHandler(TrackingController trackingController) : base((int) EventCodes.TakeSilver)
        {
            _trackingController = trackingController;
        }

        protected override async Task OnActionAsync(TakeSilverEvent value)
        {
            var localEntity = _trackingController.EntityController.GetLocalEntity()?.Value;
            if (localEntity?.ObjectId == value.ObjectId)
            {
                // TODO: Add cluster and premium bonus calculation
                _trackingController.AddNotification(SetNotification(value.YieldAfterTax.DoubleValue, 0, 0));

                _trackingController.CountUpTimer.Add(ValueType.Silver, value.YieldAfterTax.DoubleValue);
                _trackingController.DungeonController?.AddValueToDungeon(value.YieldAfterTax.DoubleValue, ValueType.Silver);
            }

            await Task.CompletedTask;
        }

        private TrackingNotification SetNotification(double totalGainedSilver, double cluster, double premium)
        {
            return new TrackingNotification(DateTime.Now, new List<LineFragment>
            {
                new SilverNotificationFragment(LanguageController.Translation("YOU_HAVE"), AttributeStatOperator.Plus, totalGainedSilver, 
                    LanguageController.Translation("SILVER"), cluster, premium, LanguageController.Translation("GAINED"))
            });
        }
    }
}