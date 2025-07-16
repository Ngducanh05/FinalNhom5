using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject volumeSliderObj;  // Đối tượng thanh kéo âm lượng
    public GameObject muteToggleObj;    // Đối tượng nút bật/tắt tiếng (checkbox)
    public GameObject backButtonObj;    // Đối tượng nút Back (quay lại)

    [Header("Audio")]
    public Slider volumeSlider;         // Thanh kéo điều chỉnh âm lượng
    public Toggle muteToggle;           // Checkbox bật/tắt tiếng
    public AudioSource bgm;             // Nhạc nền (Background Music)
    void Start()
    { 
        //khi bắt đầu
        // Ẩn các âm lượng , bật tắt mute và nút back
        volumeSliderObj.SetActive(false);
        muteToggleObj.SetActive(false);
        backButtonObj.SetActive(false);

        // Load dữ liệu âm thanh đã lưu
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        //Lấy dữ liệu âm lượng đã được lưu từ trước trong PlayerPrefs với key là "Volume".
        //nếu chưa từng lưu, nó sẽ lấy giá trị mặc định là 1f(tức 100 % âm lượng).
        //Mục đích: Khi mở game lại, âm lượng sẽ giữ nguyên như lần trước.
        muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;
        //Lấy dữ liệu trạng thái tắt tiếng (Muted) đã lưu trong PlayerPrefs.
        //Nếu giá trị là 1 → Bật mute (isOn = true).
        //Nếu giá trị là 0 → Không mute (isOn = false).
        //Nếu chưa từng lưu, mặc định là 0 (không mute).
        //Mục đích: Giữ nguyên trạng thái bật/tắt tiếng như lần chơi trước.
        ApplySound();// áp dụng ngay 
    }

    //UI
    public void OpenOption()
    {
        volumeSliderObj.SetActive(true);
        muteToggleObj.SetActive(true);
        backButtonObj.SetActive(true);//khi mở option thì các cái này sẽ hiện nên áp là true
    }

    public void CloseOption()
    {
        volumeSliderObj.SetActive(false);
        muteToggleObj.SetActive(false);
        backButtonObj.SetActive(false);// khi ấn back thì tất cả cái này sẽ bị ẩn nên áp là false
    }

    // Audio
    public void OnVolumeChange(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);// lưu giá trị âm lượng mới
        ApplySound();// áp dụng ngay 
    }

    public void OnMuteChange(bool isMuted)
    {
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);// Nếu bật mute lưu 1, tắt mute lưu 0
        ApplySound();// áp dụng ngay 
    }

    void ApplySound()
    {
        if (muteToggle.isOn)
        {
            bgm.volume = 0;// nếu bật mute thì tắt tiếng 
        }
        else
        {
            bgm.volume = volumeSlider.value; //nếu không bật mute thì sẽ dùng thanh volume để
                                             //Kéo tăng giảm âm lượng 
        }
    }
}
