using System;
using KodakkuAssist.Module.GameEvent;
using KodakkuAssist.Script;
using KodakkuAssist.Module.Draw;
using Dalamud.Utility.Numerics;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using Dalamud.Memory.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using KodakkuAssist.Module.GameOperate;

namespace KodakkuAssist.Omega;

[ScriptType(name: "Omega1", territorys: [691], guid: "e0053855-1afd-49a7-85c8-537a6c7c1273", version: "0.0.0.1",
    author: "JiaXX")]
public class Omega1
{
    [ScriptMethod(name: "火焰球", eventType: EventTypeEnum.AddCombatant, eventCondition: ["DataId:6769"])]
    public void 火焰球(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();

        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }

        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Name = $"火焰球 {@event["SourceId"]}";
        dp.Scale = new Vector2(8);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 30000;
        accessory.Method.SendDraw(0, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "火焰球清除", eventType: EventTypeEnum.ActionEffect, eventCondition: ["ActionId:9173"])]
    public void 火焰球清除(Event @event, ScriptAccessory accessory)
    {
        accessory.Method.RemoveDraw($"火焰球 {@event["SourceId"]}");
    }

    [ScriptMethod(name: "风息之翼", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9182"])]
    public void 风息之翼(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "点位";
        dp.Color = accessory.Data.DefaultSafeColor;
        dp.Scale = new Vector2(1.5f);
        dp.ScaleMode |= ScaleMode.YByDistance;
        dp.Owner = accessory.Data.Me;
        dp.TargetPosition = JsonConvert.DeserializeObject<Vector3>(@event["SourcePosition"]);
        dp.DestoryAt = 6700;
        accessory.Method.SendDraw(DrawModeEnum.Imgui, DrawTypeEnum.Displacement, dp);
    }

    [ScriptMethod(name: "双重落雷", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9175"])]
    public void 双重落雷(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "MT死刑";
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Owner = accessory.Data.PartyList[0];
        dp.Scale = new Vector2(3);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 6000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);

        dp.Name = "ST死刑";
        dp.Owner = accessory.Data.PartyList[1];
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    private static bool ParseObjectId(string? idStr, out uint id)
    {
        id = 0;
        if (string.IsNullOrEmpty(idStr)) return false;
        try
        {
            var idStr2 = idStr.Replace("0x", "");
            id = uint.Parse(idStr2, System.Globalization.NumberStyles.HexNumber);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}