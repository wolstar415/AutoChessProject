using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
public class FsmClass<T> where T : System.Enum
{
    protected Dictionary<T, FsmState<T>> m_stateList = new Dictionary<T, FsmState<T>>();
    protected FsmState<T> m_state;
    

    public FsmState<T> getState { get { return m_state; } }
    public T getStateType
    {
        get
        {
            if (null == m_state)
                return default(T);

            return m_state.stateType;
        }
    }

    public void Stop()
    {
        m_state = null;
    }

    public virtual void Init()
    {

    }

    public virtual void Clear()
    {
        m_stateList.Clear();
        m_state = null;
    }

    public virtual void AddFsm(FsmState<T> _state)
    {
        if( null == _state )
        {
            Debug.LogError("FsmClass::AddFsm()[ null == FsmState<T>");
            return;
        }

        if( true == m_stateList.ContainsKey(_state.stateType) )
        {
            Debug.LogError("FsmClass::AddFsm()[ have state : " + _state.stateType);
            return;
        }

        m_stateList.Add(_state.stateType, _state);
    }


    public virtual void SetState( T _stateType, FSMMsg _msg = null )
    {
        if( false == m_stateList.ContainsKey(_stateType))
        {
            Debug.LogError("FsmClass::SetState()[ no have state : " + _stateType);
            return;
        }



        FsmState<T> _nextState = m_stateList[_stateType];
        if( _nextState == m_state )
        {
            Debug.LogWarning("FsmClass::SetState()[ same state : " + _stateType);
        }


        if( null != m_state )
        {
            m_state.Finally();
            m_state.End();
        }

        m_state = _nextState;
        m_state.Enter(_msg);
        
    }
    /// <summary>
    /// 강제로 바꿀때 사용합니다 End 실행 X Finally만 실행
    /// </summary>
    /// <param name="_stateType"></param>
    /// <param name="_msg"></param>
    public virtual void ChaneState( T _stateType, FSMMsg _msg = null )
    {
        if( false == m_stateList.ContainsKey(_stateType))
        {
            Debug.LogError("FsmClass::SetState()[ no have state : " + _stateType);
            return;
        }

        FsmState<T> _nextState = m_stateList[_stateType];
        if( _nextState == m_state )
        {
            Debug.LogWarning("FsmClass::SetState()[ same state : " + _stateType);
        }


        if( null != m_state )
        {
            m_state.Finally();
        }

        m_state = _nextState;
        m_state.Enter(_msg);
        
    }

    public virtual void SetMsg(FSMMsg _msg)
    {
        if (m_state == null)
            return;

        if (_msg == null)
            return;

        m_state.SetMsg(_msg);
    }

    public virtual void Update()
    {
        if (null == m_state)
            return;

        m_state.Update();
    }
}
}