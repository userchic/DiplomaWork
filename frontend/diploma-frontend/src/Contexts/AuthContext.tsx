import { createContext, useState, type JSX } from "react";

type AuthContextType = {
    isAuthenticated: boolean;
    setAuth: (auth: boolean) => void
}
const AuthContext = createContext<AuthContextType>({
    isAuthenticated: localStorage.getItem("authFlag") === "true",
    setAuth: () => { }
})

export const AuthProvider = ({ children }: { children: JSX.Element }) => {
    const [isAuthenticated, setAuth] = useState<boolean>(localStorage.getItem("authFlag") === "true")
    return <AuthContext.Provider value={{ isAuthenticated, setAuth }}>
        {children}
    </AuthContext.Provider>
}
export default AuthContext;