import './GeminiChatButton.css';
import React, { useState, useEffect } from "react";
import img12 from '../assets/images/ai-assistant.png';

const GeminiChatButton = () => {
  const [open, setOpen] = useState(false);
  const [input, setInput] = useState("");
  const [chatLog, setChatLog] = useState([]);
  const [isTyping, setIsTyping] = useState(false);

  const apiKey = "AIzaSyDAimW29GdUbkIieOSRpGjdtCLrgj3SvCY";

  useEffect(() => {
    const savedChat = localStorage.getItem("gemini-chat");
    if (savedChat) {
      setChatLog(JSON.parse(savedChat));
    }
  }, []);

  useEffect(() => {
    localStorage.setItem("gemini-chat", JSON.stringify(chatLog));
  }, [chatLog]);

  useEffect(() => {
    if (open && chatLog.length === 0) {
      setChatLog([{ from: "gemini", text: "Hello! How can I help you?😊" }]);
    }
  }, [open]);

  async function sendToGemini(question) {
    try {
      const response = await fetch(
        `https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=${apiKey}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            contents: [{ parts: [{ text: question }] }],
          }),
        }
      );

      const data = await response.json();
      if (data.candidates?.length > 0) {
        return data.candidates[0].content?.parts?.[0]?.text || "No text in content";
      }
      return "No response";
    } catch (error) {
      console.error("Error contacting Gemini API:", error);
      return "An error occurred, please try again.";
    }
  }

  async function handleSend() {
    if (!input.trim()) return;

    const userMsg = { from: "user", text: input };
    setChatLog((prev) => [...prev, userMsg]);
    setInput("");
    setIsTyping(true);

    const response = await sendToGemini(input);

    setIsTyping(false);
    const botMsg = { from: "gemini", text: response };
    setChatLog((prev) => [...prev, botMsg]);
  }

  return (
    <>
      <button className="chatbot-toggle" onClick={() => setOpen(!open)}>
        <img src={img12} alt="Chatbot Icon" width="32" height="32" />
      </button>

      {open && (
        <div className="chat-container">
          <h2>AI Assistant 🤖</h2>
          <div className="messages">
            {chatLog.map((msg, idx) => (
              <div key={idx} className={`msg ${msg.from === "user" ? "user" : "bot"}`}>
                {msg.text}
              </div>
            ))}
            {isTyping && (
              <div className="msg bot typing">Writing...</div>
            )}
          </div>
          <div className="input-area">
            <input
              type="text"
              value={input}
              placeholder="Write your question here..."
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && handleSend()}
            />
            <button onClick={handleSend}>Send</button>
          </div>
        </div>
      )}
    </>
  );
};

export default GeminiChatButton;
