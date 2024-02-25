using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Handle : MonoBehaviour
{
    [Header("Obj Game")]
    public Carrot.Carrot carrot;
    public GameObject[] cloudPrefab;
    public AudioSource[] sound;
    public GameObject plane_prefab;
    public Camera cam_main;
    public Transform arean_missile;
    public Color32[] c_background;

    [Header("Panel Game")]
    public GameObject panel_play;
    public GameObject panel_main;
    public GameObject panel_gameover;
    public Text txt_scores;

    [Header("Gameover Obj")]
    public Text txt_gameover_your_scores;
    public Text txt_gameover_hight_scores;

    [Header("GamePad")]
    public List<GameObject> list_gamepad_main;
    public List<GameObject> list_gamepad_gameover;

    private int scores=0;
    private int hight_scores=0;
    private PlaneBehaviour plane;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.act_after_close_all_box = this.set_gamepad_main;

        this.panel_play.SetActive(false);
        this.panel_main.SetActive(true);
        this.panel_gameover.SetActive(false);
        this.carrot.game.load_bk_music(this.sound[0]);
        this.makeClouds();

        this.hight_scores = PlayerPrefs.GetInt("game_hight_scores", 0);
        this.create_plane();
        this.load_background();

        this.carrot.game.load_bk_music(this.sound[0]);

        Carrot.Carrot_Gamepad gamepad1=this.carrot.game.create_gamepad("plane");
        gamepad1.set_gamepad_keydown_left(this.keydown_go_left);
        gamepad1.set_gamepad_keydown_right(this.keydown_go_right);
        gamepad1.set_gamepad_keydown_up(this.gamepad_keydown_up);
        gamepad1.set_gamepad_keydown_down(this.gamepad_keydown_down);
        gamepad1.set_gamepad_keydown_start(this.gamepad_keydown_ok);
        gamepad1.set_gamepad_keydown_x(this.gamepad_keydown_ok);
        gamepad1.set_gamepad_keydown_y(this.gamepad_keydown_exit);

        this.carrot.game.set_list_button_gamepad_console(this.list_gamepad_main);
    }

    public void set_gamepad_main()
    {
        this.carrot.game.set_list_button_gamepad_console(this.list_gamepad_main);
    }

    public void create_plane()
    {
        if (this.plane != null)
        {
            Destroy(this.plane.gameObject);
            this.plane = null;
        }

        GameObject obj_place = Instantiate(this.plane_prefab);
        obj_place.transform.position = Vector3.zero;
        this.plane = obj_place.GetComponent<PlaneBehaviour>();
        this.plane.cam = this.cam_main;
        this.plane.canvas = this.panel_gameover;
        this.cam_main.GetComponent<CameraBehaviour>().obj_tip_help = this.plane.obj_tip_help;
        this.cam_main.GetComponent<CameraBehaviour>().target = this.plane.transform;
        this.cam_main.GetComponent<CameraBehaviour>().gameOver = false;
        this.cam_main.GetComponent<CameraBehaviour>().show_effect_tip();
        this.cam_main.transform.position = new Vector3(0f, 0f, -10f);
        this.cam_main.GetComponent<CameraBehaviour>().load();
    }

    private void load_background()
    {
        int index_c_bk = Random.Range(0, this.c_background.Length);
        this.cam_main.GetComponent<Camera>().backgroundColor = this.c_background[index_c_bk];
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void keydown_go_left()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.plane.go_left();
            this.play_sound(3);
        }
        else
        {
            this.carrot.game.gamepad_keydown_down_console();
        }
    }

    public void keydown_go_right()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.plane.go_right();
            this.play_sound(3);
        }
        else
        {
            this.carrot.game.gamepad_keydown_up_console();
        }
    }

    public void gamepad_keydown_up()
    {
        this.carrot.game.gamepad_keydown_up_console();
    }

    public void gamepad_keydown_down()
    {
        this.carrot.game.gamepad_keydown_down_console();
    }

    public void gamepad_keydown_ok()
    {
        this.carrot.game.gamepad_keydown_enter_console();
    }

    public void gamepad_keydown_exit()
    {
        this.carrot.act_Escape();
    }

    public void btn_play_now()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.game.set_enable_gamepad_console(false);
        this.load_background();
        this.carrot.clear_contain(this.arean_missile);
        this.create_plane();
        this.cam_main.GetComponent<CameraBehaviour>().load_missileSpawner();
        this.scores = 0;
        this.txt_scores.text = "0";
        this.carrot.play_sound_click();
        this.panel_main.SetActive(false);
        this.panel_play.SetActive(true);
        this.panel_gameover.SetActive(false);
    }

    public void btn_back_menu()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.clear_contain(this.arean_missile);
        this.create_plane();
        this.cam_main.GetComponent<CameraBehaviour>().gameOver = true;
        this.carrot.play_sound_click();
        this.panel_main.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_gameover.SetActive(false);
        this.carrot.game.set_list_button_gamepad_console(this.list_gamepad_main);
    }

    public void btn_show_setting()
    {
        Carrot.Carrot_Box box_setting = this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.before_close_setting);
        box_setting.update_gamepad_cosonle_control();
    }

    private void before_close_setting(List<string> list_item_change)
    {
        foreach(string s in list_item_change)
        {
            if (s == "bk_music") this.carrot.game.load_bk_music(this.sound[0]);
        }
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_more_app()
    {
        this.carrot.show_list_carrot_app();
    }

    public void btn_show_top_player()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_show_user_login()
    {
        this.carrot.user.show_login();
    }

    private void makeClouds()
    {
        for (int i = -40; i < 40; i += (int)Random.Range(5, 8))
        {
            for (int j = -40; j < 40; j += (int)Random.Range(5, 8))
            {
                Instantiate(cloudPrefab[Random.Range(0, cloudPrefab.Length)], new Vector3(i, j, -1f), Quaternion.identity).transform.SetParent(this.transform);
            }
        }
    }

    public void add_scores()
    {
        this.scores++;
        this.txt_scores.text = this.scores.ToString();
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void show_gameover()
    {
        this.carrot.play_vibrate();
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.clear_contain(this.arean_missile);
        if (this.scores > this.hight_scores)
        {
            this.hight_scores = this.scores;
            PlayerPrefs.SetInt("game_hight_scores", this.hight_scores);
            this.carrot.game.update_scores_player(this.hight_scores);
        }
        this.txt_gameover_hight_scores.text = this.hight_scores.ToString();
        this.txt_gameover_your_scores.text = this.scores.ToString();
        this.plane = null;
        this.cam_main.GetComponent<CameraBehaviour>().gameOver = true;
        this.panel_play.SetActive(false);
    }
}
