﻿// Copyright (C) 2019-2020 Alexander Bogarsukov. All rights reserved.
// See the LICENSE.md file in the project root for more information.

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityFx.Outline.PostProcessing
{
	[Serializable]
	[PostProcess(typeof(OutlineEffectRenderer), PostProcessEvent.BeforeStack, "UnityFx/Outline", false)]
	public sealed class Outline : PostProcessEffectSettings
	{
		[SerializeField]
		private OutlineResources _defaultResources;

		[Serializable]
		public class OutlineResourcesParameter : ParameterOverride<OutlineResources>
		{
			internal Outline Settings;

			protected override void OnEnable()
			{
				if (value == null)
				{
					value = Settings._defaultResources;
				}
			}
		}

		[Serializable]
		public class OutlineLayersParameter : ParameterOverride<OutlineLayerCollection>
		{
		}

		// NOTE: PostProcessEffectSettings.OnEnable implementation requires the fields to be public to function properly.
		public OutlineResourcesParameter Resources = new OutlineResourcesParameter();
		public OutlineLayersParameter Layers = new OutlineLayersParameter();

		public Outline()
		{
			// NOTE: The better way to do this is implementing OnEnable(), but it is already implemented as private (!!!) method in PostProcessEffectSettings.
			Resources.Settings = this;
		}

		public override bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
#if UNITY_EDITOR

			// Don't render outline preview when the editor is not playing.
			if (!Application.isPlaying)
			{
				return false;
			}

#endif

			return base.IsEnabledAndSupported(context) && Resources != null && Layers != null && Layers.value != null;
		}
	}
}
