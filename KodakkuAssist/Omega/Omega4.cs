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

[ScriptType(name: "Omega4", territorys: [694], guid: "b3d37f50-67f9-4e57-9a58-afa3861a168c", version: "0.0.0.1",
    author: "JiaXX")]
public class Omega4
{
    [ScriptMethod(name: "死亡宣告", eventType: EventTypeEnum.StatusAdd, eventCondition: ["StatusID:910"])]
    public void 死亡宣告(Event @event, ScriptAccessory accessory)
    {

        accessory.Method.TextInfo("奶妈救一下救一下啊", 2000);
    }

    [ScriptMethod(name: "热病", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9407"])]
    public void 热病(Event @event, ScriptAccessory accessory)
    {

        accessory.Method.TextInfo("停停停", 2000);
    }

    [ScriptMethod(name: "冰封", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9408"])]
    public void 冰封(Event @event, ScriptAccessory accessory)
    {

        accessory.Method.TextInfo("动动动", 2000);
    }

    [ScriptMethod(name: "黑洞", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9416"])]
    public void 黑洞(Event @event, ScriptAccessory accessory)
    {

        accessory.Method.TextInfo("远离黑洞", 2000);
    }

    [ScriptMethod(name: "暴雷死刑", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9405"])]
    public void 暴雷死刑(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "暴雷死刑";
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Owner = accessory.Data.PartyList[0];
        dp.Scale = new Vector2(5);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 5000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "暴雷AOE", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9409"])]
    public void 暴雷AOE(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        dp.Name = "暴雷AOE";
        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Scale = new Vector2(15);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 5000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "暴炎", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9402"])]
    public void 暴炎(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        if (ParseObjectId(@event["TargetId"], out var id))
        {
            dp.Owner = id;
        }
        dp.Name = "暴炎";
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Scale = new Vector2(4);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 4000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "死亡吐息", eventType: EventTypeEnum.StartCasting, eventCondition: ["ActionId:9419"])]
    public void 死亡吐息(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();
        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }
        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Scale = new Vector2(20);
        dp.Radian = float.Pi * 2 / 3;
        dp.DestoryAt = 3000;
        accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Fan, dp);
    }

    [ScriptMethod(name: "黑洞生成", eventType: EventTypeEnum.AddCombatant, eventCondition: ["DataId:7802"])]
    public void 黑洞生成(Event @event, ScriptAccessory accessory)
    {
        var dp = accessory.Data.GetDefaultDrawProperties();

        if (ParseObjectId(@event["SourceId"], out var id))
        {
            dp.Owner = id;
        }

        dp.Color = accessory.Data.DefaultDangerColor;
        dp.Name = $"黑洞 {@event["SourceId"]}";
        dp.Scale = new Vector2(2);
        dp.Radian = float.Pi * 2;
        dp.DestoryAt = 30000;
        accessory.Method.SendDraw(0, DrawTypeEnum.Circle, dp);
    }

    [ScriptMethod(name: "黑洞清除", eventType: EventTypeEnum.RemoveCombatant, eventCondition: ["DataId:7802"])]
    public void 黑洞清除(Event @event, ScriptAccessory accessory)
    {
        accessory.Method.RemoveDraw($"黑洞 {@event["SourceId"]}");
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