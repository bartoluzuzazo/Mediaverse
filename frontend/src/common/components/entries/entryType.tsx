import {FunctionComponent} from "react";

interface props {
    type: string
}

const EntryType : FunctionComponent<props> = ({type}) => {
    return (
        <div className="bg-violet-200">
            <div className="text-violet-500 text-xl pl-3 pr-3 p-1 shadow-lg">
                {type}
            </div>
        </div>
    )
}

export default EntryType;