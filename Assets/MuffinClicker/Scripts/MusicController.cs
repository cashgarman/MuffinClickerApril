using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MusicController : MonoBehaviour
{
	[FormerlySerializedAs("musicTracks")] public AudioSource[] _musicTracks;
	private AudioSource _currentTrack;
	private string[] _currentSequence;
	private int _currentIndex;
	private AudioSource _interruptingTrack;

	private void Start()
	{
		// Starting and silencing each music loop
		foreach (var loop in _musicTracks)
		{
			loop.Play();
			loop.volume = 0;
		}
	}

	private void PlayTrack(string trackName)
	{
		// If we were already playing a track
		if (_currentTrack != null)
		{
			// Put it to the background
			PutToBackground(_currentTrack);
		}

		// LINQ & Lambda Expressions / Anonymous Methods
		_currentTrack = _musicTracks.First(track => track.name == trackName);
		BringToForeground(_currentTrack);
	}

	private void Update()
	{
		// If the current track has finished
		if (_currentTrack != null && !_currentTrack.isPlaying)
		{
			// If we're in a sequence
			if (_interruptingTrack == null)
			{
				// Move to the next track in the sequence
				++_currentIndex;
			
				// Loop the sequence
				if (_currentIndex >= _currentSequence.Length)
					_currentIndex = 0;
			
				// Grab the next track in the sequence
				Debug.Log($"Sequence: {string.Join(", ", _currentSequence)}");
				var nextTrack = GetTrackByName(_currentSequence[_currentIndex]); 
			
				// Put the current track into the background
				PutToBackground(_currentTrack);

				Debug.Log($"Playing next track in sequence {nextTrack.name}");

				// Play the next track
				_currentTrack = nextTrack;
				_currentTrack.Play();
				BringToForeground(_currentTrack);
			}
			// If we're interrupting
			else
			{
				// Repeat the interrupting track
				_interruptingTrack.Play();
			}
		}
	}

	private AudioSource GetTrackByName(string trackName)
	{
		return _musicTracks.First(t => t.name == trackName);
	}

	private void BringToForeground(AudioSource track, float crossfadeTime = 0)
	{
		Debug.Log($"Bringing track {track.name} into the foreground");
		if(crossfadeTime != 0)
			track.DOFade(1f, crossfadeTime);
		else
			track.volume = 1f;
		track.loop = false;	// Turn off the looping so we are informed when the track finishes
	}

	private void PutToBackground(AudioSource track, float crossfadeTime = 0)
	{
		Debug.Log($"Putting track {track.name} into the background");
		//_currentTrack.Play();
		if(crossfadeTime != 0)
			track.DOFade(0f, crossfadeTime);
		else
			track.volume = 0f;
		track.loop = true;
	}

	public void QueueTrackLoop(params string[] tracks)
	{
		if (!tracks.Any())
			throw new Exception("You need to pass at least 1 teach");
		
		_currentSequence = tracks;
		_currentIndex = 0;
		
		PlayTrack(_currentSequence[_currentIndex]);
	}

	public void StartTrackInterrupt(string trackName)
	{
		// Put the current playing track into the background
		PutToBackground(_currentTrack, 2f);
		
		// Bring the interrupting track into the foreground
		_interruptingTrack = GetTrackByName(trackName);
		BringToForeground(_interruptingTrack, 2f);
	}

	public void StopTrackInterrupt()
	{
		// Put the interrupting track into the background
		PutToBackground(_interruptingTrack, 2f);
		
		// Bring the old current sequence to the foreground
		BringToForeground(_currentTrack, 2f);

		_interruptingTrack = null;
	}
}
