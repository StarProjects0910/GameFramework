﻿using System;
using System.Collections.Generic;

//用来模拟服务器，接收处理技能的请求，并返回结果
public class SkillProcesser
{
    public static void ProcessSkill(ReqCastSkill msg)
    {
        Entity caster = EntityManager.Instance.Find(msg.caster);
        if (caster == null)
        {
            Log.Error("找不到技能释放者!  id = " + msg.caster);
            return;
        }
        CSVSkill skillInfo = CSVManager.GetSkillCfg(msg.skillID);
        if (skillInfo == null)
        {
            Log.Error("找不到技能配置 skillid = " + msg.skillID);
            return;
        }

        List<Entity> targets = GetTargets(msg, skillInfo);
        if (targets == null)
            return;

        List<SkillResult> result = null;
        for (int i = 0; i < targets.Count; i++)
        {
            SkillResult sr = new SkillResult();
            sr.target = msg.target;
            if (skillInfo.type == 1) //普攻
            {
                sr.damage = caster.Property.PhysicDamage - targets[i].Property.PhysicDefence;
            }
            else if (skillInfo.type == 2)        //物理技能攻击
            {

            }
            else if (skillInfo.type == 3)    //法术技能攻击
            {

            }
            result.Add(sr);
        }
        OnCastSkill rsp = new OnCastSkill();
        rsp.result = result;
        caster.OnEvent(eEntityEvent.OnSkillResult, rsp);
    }

    //得到这个技能作用的目标
    private static List<Entity> GetTargets(ReqCastSkill msg, CSVSkill skillInfo)
    {
        List<Entity> targets = null;
        if (skillInfo.type == 1)     //普攻
        {
            Entity target = EntityManager.Instance.Find(msg.target);
            if (targets == null)
            {
                Log.Error("找不到普通攻击的目标  target = " + msg.target);
                return null;
            }
            targets = new List<Entity>();
            targets.Add(target);
        }
        else       //技能
        {

        }
        return targets;
    }


}