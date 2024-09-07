using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class NewCustomRuleTile : RuleTile<NewCustomRuleTile.Neighbor>
{
    public Object similarTo;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        //public const int Null = 3;
        //public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.This:
                if (other == null)
                    return false;

                if (other == this)
                {
                    return true;
                }
                else
                {
                    if (other is NewCustomRuleTile r)
                    {
                        if (r.similarTo)
                        {
                            if (this == r.similarTo)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }

                    return true;
                }
            case TilingRuleOutput.Neighbor.NotThis:
                if (other == null)
                    return true;

                if (other == this)
                {
                    return false;
                }
                else
                {
                    if (other is NewCustomRuleTile r)
                    {
                        if (r.similarTo)
                        {
                            if (this == r.similarTo)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                            return true;
                    }

                    return false;
                }
        }


        // switch (neighbor)
        // {
        //     case TilingRuleOutput.Neighbor.This:
        //         if (other == this)
        //             return true;
        //         else
        //             if (other is NewCustomRuleTile rule)
        //                 return string.IsNullOrEmpty(rule.ruleGroup);
        //             else
        //                 return other != null;
        //         
        //     case TilingRuleOutput.Neighbor.NotThis: 
        //         if (other is NewCustomRuleTile rule2)
        //             return rule2.ruleGroup != ruleGroup;
        //         return other == null;
        // }

        return true;
    }
}