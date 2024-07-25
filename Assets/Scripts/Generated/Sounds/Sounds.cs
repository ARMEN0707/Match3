using UnityEngine;
using FuryLion;

public static class Sounds
{
	public static class Music
	{

		public static AudioClip Nightwing => SoundResources.Get((int)SoundsNames.Nightwing);
	}

	public static class Sound
	{

		public static AudioClip Click => SoundResources.Get((int)SoundsNames.Click);

		public static AudioClip Match => SoundResources.Get((int)SoundsNames.Match);

		public static AudioClip SelectFirst => SoundResources.Get((int)SoundsNames.SelectFirst);

		public static AudioClip SelectSecond => SoundResources.Get((int)SoundsNames.SelectSecond);

		public static AudioClip SelectThird => SoundResources.Get((int)SoundsNames.SelectThird);

		public static AudioClip SelectFourth => SoundResources.Get((int)SoundsNames.SelectFourth);

		public static AudioClip Explosion => SoundResources.Get((int)SoundsNames.Explosion);
	}

}