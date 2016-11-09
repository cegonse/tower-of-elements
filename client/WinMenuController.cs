using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinMenuController : MonoBehaviour
{
    public Image _backgroundTexture;
    public Text _timeText;
    public Image _hourGlass;
    public Image _star1;
    public Image _star2;
    public Image _star3;
    public Image _star4;
    public Image _star5;
    public Button _menuButton;
    public Button _resetButton;
    public Button _nextButton;

    private GameObject _caller;
    private GameController _game;

    private bool _shownStar1Particles = false;
    private bool _shownStar2Particles = false;
    private bool _shownStar3Particles = false;
    private bool _shownStar4Particles = false;
    private bool _shownStar5Particles = false;

    private enum WinState
    {
        Idle,
        Init,
        ShowingBackground,
        ShowingTimer,
        WaitingTimer,
        WaitingStar1,
        WaitingStar2,
        WaitingStar3,
        WaitingStar4,
        WaitingStar5,
        ShowingButtons
    }

    private WinState _state = WinState.Idle;
    private float _timer = 0f;

    private int _numStars = 0;
    private float _playTime = 0f;

    public void OnPlayerWin(GameObject caller, GameController game, int numStars = 3, float playTime = 73f)
    {
        _game = game;
        _state = WinState.Init;
        _caller = caller;
        _numStars = numStars;
        _playTime = playTime;

        _shownStar1Particles = false;
        _shownStar2Particles = false;
        _shownStar3Particles = false;
        _shownStar4Particles = false;
        _shownStar5Particles = false;
    }

    public bool CanContinue()
    {
        return _state == WinState.Idle;
    }

    public void HideMenu()
    {
        Color cl = new Color(1f, 1f, 1f, 0f);

        _backgroundTexture.color = cl;
        _timeText.transform.localScale = Vector3.zero;
        _hourGlass.transform.localScale = Vector3.zero;
        _star1.transform.localScale = Vector3.zero;
        _star2.transform.localScale = Vector3.zero;
        _star3.transform.localScale = Vector3.zero;
        _star4.transform.localScale = Vector3.zero;
        _star5.transform.localScale = Vector3.zero;
        _menuButton.transform.localScale = Vector3.zero;
        _resetButton.transform.localScale = Vector3.zero;
        _nextButton.transform.localScale = Vector3.zero;

        _state = WinState.Idle;
    }

    void Start()
    {
        Color cl = new Color(1f, 1f, 1f, 0f);

        _backgroundTexture.color = cl;
        _timeText.transform.localScale = Vector3.zero;
        _hourGlass.transform.localScale = Vector3.zero;
        _star1.transform.localScale = Vector3.zero;
        _star2.transform.localScale = Vector3.zero;
        _star3.transform.localScale = Vector3.zero;
        _star4.transform.localScale = Vector3.zero;
        _star5.transform.localScale = Vector3.zero;
        _menuButton.transform.localScale = Vector3.zero;
        _resetButton.transform.localScale = Vector3.zero;
        _nextButton.transform.localScale = Vector3.zero;
    }

    private void CreateStarParticles(int star)
    {
        if (star == 1)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject go = new GameObject();
                go.name = "Star Particle";
                go.transform.position = _star1.transform.position;
                go.transform.parent = _star1.transform.parent;
                go.transform.SetSiblingIndex(_star1.transform.GetSiblingIndex() - 1);
                go.transform.localScale = Vector3.one * 0.5f;

                UnityEngine.UI.Image img = go.AddComponent<UnityEngine.UI.Image>();
                float sz = _game.GetTextureController().GetTextureSize("Particles/ParticleFire/ParticleFire_2");
                img.sprite = Sprite.Create((Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2"),
                                           new Rect(0, 0, sz, sz), new Vector2(0.5f, 0.5f), sz * 32f);

                DustParticle dp = go.AddComponent<DustParticle>();
                dp.StartParticle(Random.insideUnitCircle, true, 0.7f, 200f, true);
            }
        }
        else if (star == 2)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject go = new GameObject();
                go.name = "Star Particle";
                go.transform.position = _star2.transform.position;
                go.transform.parent = _star2.transform.parent;
                go.transform.SetSiblingIndex(_star1.transform.GetSiblingIndex() - 1);
                go.transform.localScale = Vector3.one * 0.5f;

                UnityEngine.UI.Image img = go.AddComponent<UnityEngine.UI.Image>();
                float sz = _game.GetTextureController().GetTextureSize("Particles/ParticleFire/ParticleFire_2");
                img.sprite = Sprite.Create((Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2"),
                                           new Rect(0, 0, sz, sz), new Vector2(0.5f, 0.5f), sz * 32f);

                DustParticle dp = go.AddComponent<DustParticle>();
                dp.StartParticle(Random.insideUnitCircle, true, 0.7f, 200f, true);
            }
        }
        else if (star == 3)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject go = new GameObject();
                go.name = "Star Particle";
                go.transform.position = _star3.transform.position;
                go.transform.parent = _star3.transform.parent;
                go.transform.SetSiblingIndex(_star1.transform.GetSiblingIndex() - 1);
                go.transform.localScale = Vector3.one * 0.5f;

                UnityEngine.UI.Image img = go.AddComponent<UnityEngine.UI.Image>();
                float sz = _game.GetTextureController().GetTextureSize("Particles/ParticleFire/ParticleFire_2");
                img.sprite = Sprite.Create((Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2"),
                                           new Rect(0, 0, sz, sz), new Vector2(0.5f, 0.5f), sz * 32f);

                DustParticle dp = go.AddComponent<DustParticle>();
                dp.StartParticle(Random.insideUnitCircle, true, 0.7f, 200f, true);
            }
        }
        else if (star == 4)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject go = new GameObject();
                go.name = "Star Particle";
                go.transform.position = _star4.transform.position;
                go.transform.parent = _star4.transform.parent;
                go.transform.SetSiblingIndex(_star1.transform.GetSiblingIndex() - 1);
                go.transform.localScale = Vector3.one * 0.5f;

                UnityEngine.UI.Image img = go.AddComponent<UnityEngine.UI.Image>();
                float sz = _game.GetTextureController().GetTextureSize("Particles/ParticleFire/ParticleFire_2");
                img.sprite = Sprite.Create((Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2"),
                                           new Rect(0, 0, sz, sz), new Vector2(0.5f, 0.5f), sz * 32f);

                DustParticle dp = go.AddComponent<DustParticle>();
                dp.StartParticle(Random.insideUnitCircle, true, 0.7f, 200f, true);
            }
        }
        else if (star == 5)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject go = new GameObject();
                go.name = "Star Particle";
                go.transform.position = _star5.transform.position;
                go.transform.parent = _star5.transform.parent;
                go.transform.SetSiblingIndex(_star1.transform.GetSiblingIndex() - 1);
                go.transform.localScale = Vector3.one * 0.5f;
                go.transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 359f), Vector3.forward);

                UnityEngine.UI.Image img = go.AddComponent<UnityEngine.UI.Image>();
                float sz = _game.GetTextureController().GetTextureSize("Particles/ParticleFire/ParticleFire_2");
                img.sprite = Sprite.Create((Texture2D)_game.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2"),
                                           new Rect(0, 0, sz, sz), new Vector2(0.5f, 0.5f), sz * 32f);

                DustParticle dp = go.AddComponent<DustParticle>();
                dp.StartParticle(Random.insideUnitCircle, true, 0.7f, 200f, true);
            }
        }
    }

    void Update ()
    {
        if (_state == WinState.Init)
        {
            Color cl = new Color(1f,1f,1f,0f);

            _backgroundTexture.color = cl;
            _timeText.transform.localScale = Vector3.zero;
            _hourGlass.transform.localScale = Vector3.zero;
            _star1.transform.localScale = Vector3.zero;
            _star2.transform.localScale = Vector3.zero;
            _star3.transform.localScale = Vector3.zero;
            _star4.transform.localScale = Vector3.zero;
            _star5.transform.localScale = Vector3.zero;
            _menuButton.transform.localScale = Vector3.zero;
            _resetButton.transform.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            _timer = 0f;

            _state = WinState.ShowingBackground;
        }
        else if (_state == WinState.ShowingBackground)
        {
            Color cl = _backgroundTexture.color;
            cl.a += Time.deltaTime * 2f;
            _backgroundTexture.color = cl;

            if (cl.a > 0.75f)
            {
                _hourGlass.transform.localScale = Vector3.one;
                _timeText.transform.localScale = Vector3.one;
                _state = WinState.ShowingTimer;
            }
        }
        else if (_state == WinState.ShowingTimer)
        {
            Vector3 sc = _timeText.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 8f;
            _timeText.transform.localScale = sc;
            _hourGlass.transform.localScale = sc;

            if (sc.x >= 0.4f)
            {
                _timeText.GetComponent<WinTimerRollController>().StartRoll();
                _game.GetAudioController().PlayChannel(16);
                _state = WinState.WaitingTimer;
            }
        }
        else if (_state == WinState.WaitingTimer)
        {
            _timer += Time.deltaTime;

            if (_timer >= 0.7f)
            {
                _timeText.GetComponent<WinTimerRollController>().ShowTime(_playTime);

                if (_numStars == 1)
                {
                    CreateStarParticles(2);
                    _state = WinState.WaitingStar2;
                }
                else if (_numStars == 2)
                {
                    CreateStarParticles(4);
                    _state = WinState.WaitingStar4;
                }
                else if (_numStars == 3)
                {
                    CreateStarParticles(1);
                    _state = WinState.WaitingStar1;
                }
            }
        }
        else if (_state == WinState.WaitingStar1)
        {
            Vector3 sc = _star1.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star1.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                CreateStarParticles(2);
                _game.GetAudioController().PlayChannel(12);
                _state = WinState.WaitingStar2;
            }
        }
        else if (_state == WinState.WaitingStar2)
        {
            Vector3 sc = _star2.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star2.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _game.GetAudioController().PlayChannel(13);

                if (_numStars == 1)
                {
                    _state = WinState.ShowingButtons;
                }
                else if (_numStars == 3)
                {
                    CreateStarParticles(3);
                    _state = WinState.WaitingStar3;
                }
            }
        }
        else if (_state == WinState.WaitingStar3)
        {
            Vector3 sc = _star3.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star3.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _game.GetAudioController().PlayChannel(14);
                _state = WinState.ShowingButtons;
            }
        }
        else if (_state == WinState.WaitingStar4)
        {
            Vector3 sc = _star4.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star4.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _game.GetAudioController().PlayChannel(12);
                CreateStarParticles(5);
                _state = WinState.WaitingStar5;
            }
        }
        else if (_state == WinState.WaitingStar5)
        {
            Vector3 sc = _star5.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star5.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _game.GetAudioController().PlayChannel(13);
                _state = WinState.ShowingButtons;
            }
        }
        else if (_state == WinState.ShowingButtons)
        {
            Vector3 sc = _menuButton.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;

            _menuButton.transform.localScale = sc;
            _nextButton.transform.localScale = sc;
            _resetButton.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.Idle;
            }
        }
	}
}
