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

[ScriptType(name: "Omega2", territorys: [692], guid: "f5018ffb-0a41-476f-8966-12f81e83e7b1", version: "0.0.0.1",
    author: "JiaXX")]
public class Omega2
{
    private uint parse = 0;

    public void Init(ScriptAccessory accessory)
    {
        parse = 0;
        accessory.Method.MarkClear();
    }

    [ScriptMethod(name: "大地震", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9490"])]
    public void 大地震(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Position = new Vector3(0, 0, 0);
        dp.Scale = new Vector2(20);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 4700;
        accessory.Method.TextInfo("浮空", 2000);
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "下陷", eventType: EventTypeEnum.StatusAdd, eventCondition: ["StatusID:567"])]
    public void 下陷(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Position = new Vector3(0, 0, 0);
        dp.Scale = new Vector2(20);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 5000;
        accessory.Method.TextInfo("浮空", 2000);
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "重力操纵", eventType: EventTypeEnum.ActionEffect, eventCondition: ["ActionId:9477"])]
    public void 重力操纵(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Color = accessory.Data.DefaultDangerColor;
        if (ParseObjectId(@event["TargetId"], out var id))
        {
            dp.Owner = id;
        }

        dp.Scale = new Vector2(6);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 5000;
        accessory.Method.TextInfo("浮空", 2000);
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "侵入", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9616"])]
    public void 侵入(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Color = accessory.Data.DefaultDangerColor;
        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }

        dp.Scale = new Vector2(4);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 2700;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "主震", eventType: EventTypeEnum.ActionEffect, eventCondition: ["ActionId:9381"])]
    public void 主震(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Color = accessory.Data.DefaultDangerColor;
        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }

        dp.Scale = new Vector2(15);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 1000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "暗黑光", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9474"])]
    public void 暗黑光(Event @event, ScriptAccessory accessory)
    {
        switch (parse)
        {
            case 0:
                accessory.Method.TextInfo("浮空", 1000);
                break;
            case 1:
                accessory.Method.TextInfo("不要浮空", 1000);
                break;
        }

        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "安全点位";
        dp.Color = accessory.Data.DefaultSafeColor;
        dp.Scale = new Vector2(1.5f);
        dp.ScaleMode |= ScaleMode.YByDistance;
        dp.Owner = accessory.Data.Me;
        dp.TargetPosition = new Vector3(0, 0, 0);
        dp.DestoryAt = 10000;
        accessory.Method.SendDraw(DrawModeEnum.Imgui, DrawTypeEnum.Displacement, dp);
        parse++;
    }

    [ScriptMethod(name: "恶魔之瞳", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9485"])]
    public void 恶魔之瞳(Event @event, ScriptAccessory accessory)
    {
        accessory.Method.TextInfo("背对BOSS", 1000);
    }

    [ScriptMethod(name: "嗷嗷嗷", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9471"])]
    public void 嗷嗷嗷(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "点位";
        dp.Color = accessory.Data.DefaultSafeColor;
        dp.Scale = new Vector2(1.5f);
        dp.ScaleMode |= ScaleMode.YByDistance;
        dp.Owner = accessory.Data.Me;
        dp.TargetPosition = new Vector3(0, 0, -18);
        dp.DestoryAt = 7700;
        accessory.Method.SendDraw(DrawModeEnum.Imgui, DrawTypeEnum.Displacement, dp);
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