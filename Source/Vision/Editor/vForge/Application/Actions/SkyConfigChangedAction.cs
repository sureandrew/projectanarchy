/*
 *
 * Confidential Information of Telekinesys Research Limited (t/a Havok). Not for disclosure or distribution without Havok's
 * prior written consent. This software contains code, techniques and know-how which is confidential and proprietary to Havok.
 * Product and Trade Secret source code contains trade secrets of Havok. Havok Software (C) Copyright 1999-2013 Telekinesys Research Limited t/a Havok. All Rights Reserved. Use of this software is subject to the terms of an end user license agreement.
 *
 */

using System;
using CSharpFramework;
using CSharpFramework.Scene;
using System.Runtime.Serialization;

namespace Editor.Actions
{
	/// <summary>
	/// Action that is spawned to change the sky config
	/// </summary>
  [Serializable]
	public class SkyConfigChangedAction : IAction
	{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="_v3dLayer"></param>
    /// <param name="_newConfig"></param>
    public SkyConfigChangedAction(V3DLayer _v3dLayer, SkyConfig _newConfig)
		{
      v3dLayer = _v3dLayer;
      oldConfig = v3dLayer.SkyConfig;
      newConfig = _newConfig;
    }

    #region IAction Implementations

    /// <summary>
    /// IAction function
    /// </summary>
    public override void Do()
    {
      v3dLayer.Modified = true;
      v3dLayer.SkyConfig = newConfig; // sets Active field
      newConfig.Update();

      // Send layer change event to force refresh the shown property grid values
      IScene.SendLayerChangedEvent(new LayerChangedArgs(v3dLayer, null, LayerChangedArgs.Action.PropertyChanged));
    }

    /// <summary>
    /// IAction function
    /// </summary>
    public override void Undo()
    {
      v3dLayer.Modified = true;
      v3dLayer.SkyConfig = oldConfig;
      oldConfig.Update();
      IScene.SendLayerChangedEvent(new LayerChangedArgs(v3dLayer, null, LayerChangedArgs.Action.PropertyChanged));
    }

    /// <summary>
    /// IAction function
    /// </summary>
    public override bool Valid
    {
      get
      {
        if (!base.Valid)
          return false;
        return v3dLayer.Modifiable;
      }
    }

    /// <summary>
    /// IAction function
    /// </summary>
    public override string ShortName { get {return "Changed Sky config";}}

    #endregion

    #region ISerializable Members

    protected SkyConfigChangedAction(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      v3dLayer = EditorManager.Project.Scene.MainLayer as V3DLayer;
      oldConfig = (SkyConfig)info.GetValue("oldConfig", typeof(SkyConfig));
      newConfig = (SkyConfig)info.GetValue("newConfig", typeof(SkyConfig));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("oldConfig", oldConfig);
      info.AddValue("newConfig", newConfig);
    }

    #endregion

    V3DLayer v3dLayer;
    SkyConfig oldConfig;
    SkyConfig newConfig;
	}
}

/*
 * Havok SDK - Base file, BUILD(#20130624)
 * 
 * Confidential Information of Havok.  (C) Copyright 1999-2013
 * Telekinesys Research Limited t/a Havok. All Rights Reserved. The Havok
 * Logo, and the Havok buzzsaw logo are trademarks of Havok.  Title, ownership
 * rights, and intellectual property rights in the Havok software remain in
 * Havok and/or its suppliers.
 * 
 * Use of this software for evaluation purposes is subject to and indicates
 * acceptance of the End User licence Agreement for this product. A copy of
 * the license is included with this software and is also available from salesteam@havok.com.
 * 
 */
