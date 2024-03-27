using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"DOTween.dll",
		"Luban.Runtime.dll",
		"Protobuf.dll",
		"UnityEngine.CoreModule.dll",
		"YooAsset.dll",
		"custom.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Google.Protobuf.Collections.RepeatedField.<GetEnumerator>d__28<object>
	// Google.Protobuf.Collections.RepeatedField<object>
	// Google.Protobuf.FieldCodec.<>c<object>
	// Google.Protobuf.FieldCodec.<>c__32<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass32_0<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass38_0<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass39_0<object>
	// Google.Protobuf.FieldCodec.InputMerger<object>
	// Google.Protobuf.FieldCodec.ValuesMerger<object>
	// Google.Protobuf.FieldCodec<object>
	// Google.Protobuf.IDeepCloneable<object>
	// Google.Protobuf.IMessage<object>
	// Google.Protobuf.MessageParser.<>c__DisplayClass2_0<object>
	// Google.Protobuf.MessageParser<object>
	// Google.Protobuf.ValueReader<object>
	// Google.Protobuf.ValueWriter<object>
	// System.Action<UnityEngine.Vector3>
	// System.Action<float>
	// System.Action<int>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<object,int>
	// System.Func<object,object>
	// System.Func<object>
	// System.IEquatable<object>
	// System.Nullable<UnityEngine.Vector3>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// UnityEngine.Events.InvokableCall<int>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,int>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Playables.ScriptPlayable<object>
	// }}

	public void RefMethods()
	{
		// object BindPackage.AS<object>()
		// object DG.Tweening.TweenSettingsExtensions.OnUpdate<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetRelative<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetSpeedBased<object>(object)
		// Google.Protobuf.FieldCodec<object> Google.Protobuf.FieldCodec.ForMessage<object>(uint,Google.Protobuf.MessageParser<object>)
		// object Google.Protobuf.ProtoPreconditions.CheckNotNull<object>(object,string)
		// string Luban.StringUtil.CollectionToString<int>(System.Collections.Generic.IEnumerable<int>)
		// string Luban.StringUtil.CollectionToString<object>(System.Collections.Generic.IEnumerable<object>)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,ProtoTest.<OneKeyLogin>d__21>(Cysharp.Threading.Tasks.UniTask.Awaiter&,ProtoTest.<OneKeyLogin>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,ProtoTest.<SendProto>d__18>(Cysharp.Threading.Tasks.UniTask.Awaiter&,ProtoTest.<SendProto>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ProtoTest.<OneKeyLogin>d__21>(ProtoTest.<OneKeyLogin>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ProtoTest.<SendProto>d__18>(ProtoTest.<SendProto>d__18&)
		// object UnityEngine.Component.GetComponent<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.Component.TryGetComponent<object>(object&)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.GameObject.TryGetComponent<object>(object&)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// int UnityEngine.Playables.PlayableExtensions.AddInput<UnityEngine.Animations.AnimationMixerPlayable,UnityEngine.Playables.Playable>(UnityEngine.Animations.AnimationMixerPlayable,UnityEngine.Playables.Playable,int,float)
		// int UnityEngine.Playables.PlayableExtensions.AddInput<UnityEngine.Playables.Playable,UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,UnityEngine.Playables.Playable,int,float)
		// System.Void UnityEngine.Playables.PlayableExtensions.ConnectInput<UnityEngine.Animations.AnimationMixerPlayable,UnityEngine.Playables.Playable>(UnityEngine.Animations.AnimationMixerPlayable,int,UnityEngine.Playables.Playable,int,float)
		// System.Void UnityEngine.Playables.PlayableExtensions.ConnectInput<UnityEngine.Playables.Playable,UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,int,UnityEngine.Playables.Playable,int,float)
		// System.Void UnityEngine.Playables.PlayableExtensions.Destroy<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// UnityEngine.Playables.PlayableGraph UnityEngine.Playables.PlayableExtensions.GetGraph<UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Animations.AnimationMixerPlayable)
		// UnityEngine.Playables.PlayableGraph UnityEngine.Playables.PlayableExtensions.GetGraph<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// int UnityEngine.Playables.PlayableExtensions.GetInputCount<UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Animations.AnimationMixerPlayable)
		// int UnityEngine.Playables.PlayableExtensions.GetInputCount<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// float UnityEngine.Playables.PlayableExtensions.GetInputWeight<UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Animations.AnimationMixerPlayable,int)
		// UnityEngine.Playables.PlayState UnityEngine.Playables.PlayableExtensions.GetPlayState<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Pause<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Pause<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Play<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Play<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetInputCount<UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Animations.AnimationMixerPlayable,int)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetInputCount<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,int)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetInputWeight<UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Animations.AnimationMixerPlayable,int,float)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetInputWeight<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,int,float)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetSpeed<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable,double)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetTime<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable,double)
		// bool UnityEngine.Playables.PlayableGraph.Connect<UnityEngine.Playables.Playable,UnityEngine.Animations.AnimationMixerPlayable>(UnityEngine.Playables.Playable,int,UnityEngine.Animations.AnimationMixerPlayable,int)
		// bool UnityEngine.Playables.PlayableGraph.Connect<UnityEngine.Playables.Playable,UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,int,UnityEngine.Playables.Playable,int)
		// int UnityEngine.Playables.PlayableOutputExtensions.GetSourceOutputPort<UnityEngine.Animations.AnimationPlayableOutput>(UnityEngine.Animations.AnimationPlayableOutput)
		// System.Void UnityEngine.Playables.PlayableOutputExtensions.SetSourcePlayable<UnityEngine.Animations.AnimationPlayableOutput,UnityEngine.Playables.Playable>(UnityEngine.Animations.AnimationPlayableOutput,UnityEngine.Playables.Playable)
		// object UnityEngine.Resources.Load<object>(string)
		// UnityEngine.ResourceRequest UnityEngine.Resources.LoadAsync<object>(string)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetSync<object>(string)
		// string string.Join<int>(string,System.Collections.Generic.IEnumerable<int>)
		// string string.Join<object>(string,System.Collections.Generic.IEnumerable<object>)
		// string string.JoinCore<int>(System.Char*,int,System.Collections.Generic.IEnumerable<int>)
		// string string.JoinCore<object>(System.Char*,int,System.Collections.Generic.IEnumerable<object>)
	}
}