//  Copyright (c) 2020-present amlovey
//  
#if PLAYMAKER
using HutongGames.PlayMaker;

namespace Yade.Runtime.PlayMaker
{
    [ActionCategory("Yade Sheet")]
    [Tooltip("Get value of a cell")]
    public class GetCellValue : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmObject cell;

        [UIHint(UIHint.Variable)]
        public FsmString value;

        public override void OnEnter()
        {
            value.Value = (cell.Value as FsmCell).Value.GetValue();
            Finish();
        }
    }

    [ActionCategory("Yade Sheet")]
    [Tooltip("Get raw value of a cell")]
    public class GetCellRawValue: FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmObject cell;

        [UIHint(UIHint.Variable)]
        public FsmString value;

        public override void OnEnter()
        {
            value.Value = (cell.Value as FsmCell).Value.GetRawValue();
            Finish();
        }
    }

    [ActionCategory("Yade Sheet")]
    [Tooltip("Get unity object value of a cell")]
    public class GetCellUnityObject : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmObject cell;

        [UIHint(UIHint.Variable)]
        public FsmObject value;

        public override void OnEnter()
        {
            value.Value = (cell.Value as FsmCell).Value.GetUnityObject();
            Finish();
        }
    }
}
#endif