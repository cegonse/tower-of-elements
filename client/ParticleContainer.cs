using UnityEngine;
using System.Collections.Generic;

public struct Particle
{
    public GameObject Instance;
    public bool InUse;
}

public class ParticleContainer
{
    public const int MAX_PARTICLES = 600;

    private GameController _game;
    private List<Particle> _sparkParticles;
    private Texture2D[] _sparkParticlesTextures;

    public ParticleContainer(GameController game)
    {
        _game = game;
        _sparkParticles = new List<Particle>();

        _sparkParticlesTextures = new Texture2D[1];
        _sparkParticlesTextures[0] = (Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleIce/ParticleIce");

        GameObject particleParent = new GameObject();
        particleParent.name = "Spark Particle Container";
        particleParent.transform.position = Vector3.zero;

        for (int i = 0; i < MAX_PARTICLES; ++i)
        {
            GameObject go = new GameObject();
            go.name = "Spark Particle";
            go.transform.position = new Vector3(1000f, 0f);
            go.transform.parent = particleParent.transform;

            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(_sparkParticlesTextures[0], new Rect(0, 0, _sparkParticlesTextures[0].width, _sparkParticlesTextures[0].height),
                        new Vector2(0.5f, 0.5f), 512f);
            rend.sprite = spr;
            rend.sortingOrder = 106 + i;

            SparkParticle sp = go.AddComponent<SparkParticle>();
            sp.enabled = false;

            Particle p = new Particle();
            p.InUse = false;
            p.Instance = go;
            _sparkParticles.Add(p);
        }
    }

    public GameObject GetSparkParticle()
    {
        Particle res = new Particle();

        for (int i = 0; i < _sparkParticles.Count; ++i)
        {
            Particle p = _sparkParticles[i];

            if (!p.InUse)
            {
                res = p;
                p.InUse = true;
                p.Instance.GetComponent<SparkParticle>().enabled = true;
                _sparkParticles[i] = p;

                break;
            }
        }

        return res.Instance;
    }

    public void OnParticleFinished(GameObject p)
    {
        p.transform.position = new Vector3(1000f, 0f);

        for (int i = 0; i < _sparkParticles.Count; ++i)
        {
            Particle pr = _sparkParticles[i];

            if (pr.Instance == p)
            {
                pr.InUse = false;
                pr.Instance.GetComponent<SparkParticle>().enabled = false;
                _sparkParticles[i] = pr;
                break;
            }
        }
    }
}
