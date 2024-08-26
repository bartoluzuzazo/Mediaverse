import {FunctionComponent} from "react";

interface props {
    title: string
}

const entrySectionHeader : FunctionComponent<props> = ({title, content}) => {
    return (
        <>
            <h3 className="text-slate-500 text-xl p-4 font-bold">
                {title}
            </h3>
            <div className="border-t-slate-500 border border-b-0 border-l-0 border-r-0">
            </div>
        </>
    )
}

export default entrySectionHeader;