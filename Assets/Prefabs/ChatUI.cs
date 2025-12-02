using TMPro;
using UnityEngine;
public class ChatUI : MonoBehaviour
{
    public TextMeshProUGUI allChatContent;
    public TMP_InputField inputField;
    string chanelName = "Kênh thế giới";

    public string nickname;
    public string sendToNickName;
    public string CurrentChannel => chanelName;

    public void SendChatMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            string text = inputField.text;
            ChatManager.instance.SendChatMessage(CurrentChannel, text);
            inputField.text = "";
        }
    }

    public void SwitchChannel()
    {
        if (chanelName == "Kênh thế giới")
            chanelName = "Kênh bang hội";
        else
            chanelName = "Kênh thế giới";

        Debug.Log("Switched to channel: " + chanelName);

        // 🔥 Quan trọng nhất: cập nhật UI theo channel mới
        allChatContent.text = ChatManager.instance.GetChannelContent(chanelName);
    }

}