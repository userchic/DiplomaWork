import {useContext} from "react"
import AuthContext from "../Contexts/AuthContext" 

function useAuth(){
    return (
        useContext(AuthContext)
    )
}

export default useAuth