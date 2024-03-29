﻿using System;
namespace RayAudio {
	public class MusicStream : IDisposable {
		internal UIntPtr NativePtr;
		internal float VolumeCache = 1f;
		internal float PitchCache = 1f;

		internal MusicStream(UIntPtr ptr) {
			NativePtr = ptr;
		}

		public static MusicStream Load(string path) {
			AudioDevice.CheckState();

			return new MusicStream(Native.RHLoadMusicStream(path));
		}

		public void Play() {
			Native.RHPlayMusicStream(NativePtr);
		}

		public void Stop() {
			Native.StopMusicStream(NativePtr);
		}

		public void Pause() {
			Native.RHPauseMusicStream(NativePtr);
		}

		public void Resume() {
			Native.RHResumeMusicStream(NativePtr);
		}

		public void Update() {
			Native.UpdateMusicStream(NativePtr);
		}

		public void Dispose() {
			Native.RHUnloadMusicStream(NativePtr);
		}

		public void Seek(float secs) {
			if (secs > Length) secs = Length;
			if (!Native.EXVorbisSeek(NativePtr, secs)) throw new InvalidOperationException($"Can only seek OGG Vorbis streams.");
		}

		public bool IsPlaying {
			get {
				return Native.RHIsMusicPlaying(NativePtr);
			}
		}

		public float Volume {
			get { return VolumeCache; }
			set {
				VolumeCache = value;
				Native.RHSetMusicVolume(NativePtr, value);
			}
		}

		public float Pitch { 
			get { return PitchCache; }
			set
			{
				PitchCache = value;
				Native.RHSetMusicPitch(NativePtr, value);
			}
		}

		public float Length { get { return Native.RHGetMusicTimeLength(NativePtr); } }
		public float TimePlayed { get { return Native.RHGetMusicTimePlayed(NativePtr); } }
	}
}
