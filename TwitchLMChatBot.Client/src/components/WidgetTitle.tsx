import {ReactNode} from "react";

export function WidgetTitle({children}: { children: ReactNode }): JSX.Element {
    return <div className=" flex-grow-1">
        <div className="text-2xl font-bold">{children}</div>
    </div>;
}