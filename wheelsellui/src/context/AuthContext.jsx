// src/context/AuthContext.jsx
import React, { createContext, useContext, useState, useEffect } from 'react';
import apiClient from '../api/apiClient';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    // Состояние теперь хранит токен
    const [auth, setAuth] = useState({ 
        isAuthenticated: false, 
        user: null 
    });
    const [loading, setLoading] = useState(true);

    // 1. Инициализация: Проверка токена при загрузке страницы
    useEffect(() => {
        const storedToken = localStorage.getItem('authToken');
        if (storedToken) {
            // Если токен есть, считаем пользователя аутентифицированным
            // В боевом проекте нужно декодировать JWT, чтобы получить email
            setAuth({ 
                isAuthenticated: true, 
                user: { email: 'authenticated user' } // Моковые данные
            });
        }
        setLoading(false);
    }, []);

    // 2. Функция входа
    const login = async (email, password) => {
        try {
            // POST /api/auth/login возвращает { Token: "..." }
            const response = await apiClient.post('/auth/login', { email, password });
            const token = response.data.token; // 🛑 ВАЖНО: Получаем токен из ответа!
            
            if (token) {
                localStorage.setItem('authToken', token); // 💾 Сохраняем токен
                setAuth({ 
                    isAuthenticated: true, 
                    user: { email } 
                });
                return true;
            }
        } catch (error) {
            console.error('Login failed:', error);
            // Обработка ошибки 401
            throw new Error('Invalid email or password.'); 
        }
        return false;
    };

    // 3. Функция выхода
    const logout = () => {
        localStorage.removeItem('authToken'); // 🗑️ Удаляем токен
        setAuth({ isAuthenticated: false, user: null });
    };

    if (loading) {
        return <div>Loading...</div>; // или MUI CircularProgress
    }

    return (
        <AuthContext.Provider value={{ ...auth, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);