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

[ScriptType(name: "Omega3", territorys: [693], guid: "893c2dfc-cf25-4685-a750-9211c5c3610c", version: "0.0.0.1",
    author: "JiaXX")]
public class Omega3
{
    [ScriptMethod(name: "呱呱呱", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9318"])]
    public void 呱呱呱(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Scale = new Vector2(20);
        dp.Radian = float.Pi * 5 / 6;
        dp.DestoryAt = 4000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Fan, dp);
    }

    [ScriptMethod(name: "游戏开始", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9325"])]
    public void 游戏开始(Event @event, ScriptAccessory accessory)
    {
        accessory.Method.TextInfo("去职能对应的方格", 2000);
    }

    [ScriptMethod(name: "女王之舞", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9329"])]
    public void 女王之舞(Event @event, ScriptAccessory accessory)
    {
        accessory.Method.TextInfo("去蓝色方格", 2000);
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