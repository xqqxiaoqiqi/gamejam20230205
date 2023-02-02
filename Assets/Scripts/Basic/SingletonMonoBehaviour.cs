using UnityEngine;

/// <summary>
/// Used as tag to indicate the singleton should automatically create instance itself.
/// </summary>
public interface ISingletonAutoCreate { }

/// <summary>
/// Basic implementation of Singleton MonoBehaviour, inherits from this class to make your own singleton.
/// **NOTICE** SingletonMonoBehaviour won't create itself automatically unless it inherits from
///            |ISingletonAutoCreate|.
///
/// An example:
///
/// class MySingletonMonoBehaviour : SingletonMonoBehaviour<MySingletonMonoBehaviour>, ISingletonAutoCreate
/// {
///     protected override void OnInit() { /*...*/ }
///     protected override void OnDuplicated() { /*...*/ }
///
///     public void DoSomething() { /*...*/ }
/// }
///
/// MySingletonMonoBehaviour.Instance.DoSomething();
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T s_instance;

	public static T instance {
		get {
			if (s_instance != null) {
				return s_instance;
			}

			T[] instances = FindObjectsOfType<T>();
			if (instances.Length > 1) {
				Debug.LogError($"There should be never more than one singleton {typeof(T).Name} in this scene.");
			} else if (instances.Length == 1) {
				s_instance = instances[0];
			} else if (typeof(ISingletonAutoCreate).IsAssignableFrom(typeof(T))) {
				var obj = new GameObject("~" + typeof(T).Name);
				s_instance = obj.AddComponent<T>();
			}

			return s_instance;
		}
	}

	public static T instanceOrNull => s_instance;

	public static bool hasInstance => s_instance != null;

	protected virtual void OnInit() { }

	protected virtual void OnDuplicated() {
		Destroy(gameObject);
		Debug.LogError($"{name} is a singleton, please delete duplicated instance.");
	}

	protected void Awake() {
		if (s_instance != null && s_instance != this) {
			OnDuplicated();
		} else {
			s_instance = this as T;
			OnInit();
		}
	}

	protected virtual void OnDestroy() {
		if (s_instance == this) {
			s_instance = null;
		}
	}
}