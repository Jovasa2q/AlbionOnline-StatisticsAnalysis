﻿using Albion.Network;
using StatisticsAnalysisTool.Common;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StatisticsAnalysisTool.Network.Events
{
    public class NewEquipmentItemEvent : BaseEvent
    {
        public NewEquipmentItemEvent(Dictionary<byte, object> parameters) : base(parameters)
        {
            ConsoleManager.WriteLineForNetworkHandler(GetType().Name, parameters);

            try
            {
                if (parameters.ContainsKey(1)) ItemIndex = parameters[1].ObjectToInt();

                if (parameters.ContainsKey(8))
                {
                    var valueType = parameters[8].GetType();
                    if (valueType.IsArray && typeof(byte[]).Name == valueType.Name)
                    {
                        var spells = ((byte[]) parameters[8]).ToDictionary();
                        foreach (var spell in spells) SpellDictionary.Add(spell.Key, spell.Value.ObjectToInt());
                    }
                    else if (valueType.IsArray && typeof(short[]).Name == valueType.Name)
                    {
                        var spells = ((short[]) parameters[8]).ToDictionary();
                        foreach (var spell in spells) SpellDictionary.Add(spell.Key, spell.Value.ObjectToInt());
                    }
                    else if (valueType.IsArray && typeof(int[]).Name == valueType.Name)
                    {
                        var spells = ((int[]) parameters[8]).ToDictionary();
                        foreach (var spell in spells) SpellDictionary.Add(spell.Key, spell.Value.ObjectToInt());
                    }
                }
            }
            catch (Exception e)
            {
                ConsoleManager.WriteLineForError(MethodBase.GetCurrentMethod().DeclaringType, e);
            }
        }

        public int ItemIndex { get; }
        public Dictionary<int, int> SpellDictionary { get; } = new Dictionary<int, int>();
    }
}